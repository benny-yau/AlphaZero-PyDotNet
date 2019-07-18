
import clr
clr.AddReference("GameProject")
from ConnectFour import Game, GameState
from funcs import playMatchesBetweenVersions, playMatches
import loggers as lg

run_version = 1
ai_player_version = 206
env = Game()
episodes = 1
firstRun = True

def play_one_round():
    global firstRun
    global player1
    global player2
    go_first = input('Do you want to go first (y/n)?')
    isFirst = 0
    if (go_first.lower() == "y"):
        isFirst = 1
    else:
        isFirst = -1
    player1_version = -1
    player2_version = ai_player_version
        
    if (firstRun):
        firstRun = False;
        _, _, _, _, player1, player2 = playMatchesBetweenVersions(env, run_version, player1_version, player2_version, episodes, lg.logger_play_game, 0, isFirst)
    else:
        playMatches(player1, player2, episodes, lg.logger_play_game, 0, None, isFirst)

print ("Playing %s against AI." % Game.Name)

while True:
    play_one_round()
    play_again = input('Do you want to play again (y/n)?')
    if play_again.lower() != 'y':
        print ("Good Bye..")
        break