from flask import Flask, jsonify
import constants
import random
from steamworks_wrapper import steamworks_wrapper
from twitch_wrapper import twitch_wrapper

app = Flask(__name__)

games = {}
PLAYERS_COUNT = 'PlayersCount'
CPU_USAGE = 'CPUUsage'
MAX_CPU = 'MaxCPU'
RAM_USAGE = 'RAMUsage'
RAM_SIZE = 'RAMSize'
GAME_NAME = 'GameName'
SOURCE = 'Source'
twitch_wrapper.set_access_data(constants.TWITCH_CLIENT_ID, constants.TWITCH_API_SECRET)


@app.route('/GetData/<game_name>', methods=['GET'])
def get_data(game_name):
    # Replace this with your actual data retrieval logic
    if game_name not in games.keys():
        games[game_name] = {}
        games[game_name][MAX_CPU] = random.randint(4, 24)
        games[game_name][RAM_SIZE] = random.randint(4, 64)
        games[game_name][GAME_NAME] = game_name
    get_data_estimate(game_name)
    return jsonify(games[game_name])


def get_data_estimate(game_name):
    try:
        players_count = steamworks_wrapper.get_game_player_count(game_name)
        games[game_name][PLAYERS_COUNT] = players_count
        games[game_name][SOURCE] = 'Steam'
    except:
        players_count = None
    if players_count is None or players_count == 0:
        try:
            info = twitch_wrapper.get_games_info_by_name(game_name)
        except:
            info = None
        if info is not None and 'viewer_count' in info.keys():
            players_count = info['viewer_count'] * 70
            games[game_name][SOURCE] = 'Twitch based estimate'
        else:
            players_count = random.randint(0, constants.MAX_PLAYES_COUNT)
            games[game_name][SOURCE] = 'Random'

    games[game_name][PLAYERS_COUNT] = players_count
    # add to CPU usage
    viewer_percentage = games[game_name][PLAYERS_COUNT] / constants.MAX_PLAYES_COUNT
    if viewer_percentage > 0.8:
        cpu_usage = random.uniform(0.75, 0.99)
    elif viewer_percentage > 0.6:
        cpu_usage = random.uniform(0.55, 0.85)
    elif viewer_percentage > 0.4:
        cpu_usage = random.uniform(0.35, 0.65)
    else:
        cpu_usage = random.uniform(0.1, 0.45)

    # add to RAM usage
    if games[game_name][RAM_SIZE] < 8:
        ram_usage = games[game_name][RAM_SIZE] * 0.8
    elif games[game_name][RAM_SIZE] < 16:
        ram_usage = games[game_name][RAM_SIZE] * 0.6
    elif games[game_name][RAM_SIZE] < 32:
        ram_usage = games[game_name][RAM_SIZE] * 0.4
    else:
        ram_usage = games[game_name][RAM_SIZE] * 0.2

    games[game_name][CPU_USAGE] = cpu_usage
    games[game_name][RAM_USAGE] = ram_usage

    return players_count, cpu_usage, ram_usage


if __name__ == '__main__':
    app.run(debug=True, port=5000)
