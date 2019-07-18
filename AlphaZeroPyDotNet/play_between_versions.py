import clr
clr.AddReference("GameProject")
from ConnectFour import Game
from funcs import playMatchesBetweenVersions
import loggers as lg

env = Game()

playMatchesBetweenVersions(env, 1, 93, 206, 20, lg.logger_tourney, 0, 1)