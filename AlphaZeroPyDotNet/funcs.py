import numpy as np
import random
from ConnectFour import Game, GameState
import loggers as lg

from model import Residual_CNN

from agent import Agent, User

import config

def playMatchesBetweenVersions(env, run_version, player1version, player2version, EPISODES, logger, turns_until_tau0, goes_first = 0):
    if player1version == -1:
        player1 = User('player1', Game.StateSize, Game.ActionSize)
    else:
        player1_NN = Residual_CNN(config.REG_CONST, config.LEARNING_RATE, Game.InputShape, Game.ActionSize, config.HIDDEN_CNN_LAYERS)

        if player1version > 0:
            player1_network = player1_NN.read(Game.Name, run_version, player1version)
            player1_NN.model.set_weights(player1_network.get_weights())   
        player1 = Agent('player1', Game.StateSize, Game.ActionSize, config.MCTS_SIMS, config.CPUCT, player1_NN)

    if player2version == -1:
        player2 = User('player2', Game.StateSize, Game.ActionSize)
    else:
        player2_NN = Residual_CNN(config.REG_CONST, config.LEARNING_RATE, Game.InputShape, Game.ActionSize, config.HIDDEN_CNN_LAYERS)
        
        if player2version > 0:
            player2_network = player2_NN.read(Game.Name, run_version, player2version)
            player2_NN.model.set_weights(player2_network.get_weights())
        player2 = Agent('player2', Game.StateSize, Game.ActionSize, config.MCTS_SIMS, config.CPUCT, player2_NN)

    scores, memory, points, sp_scores = playMatches(player1, player2, EPISODES, logger, turns_until_tau0, None, goes_first)

    return (scores, memory, points, sp_scores, player1, player2)

def render(env, logger):
	npBoard = np.fromiter(env.GameState.Board, dtype = int)
	for r in range(6):
		msg = [GameState.Pieces[str(x)] for x in npBoard[7*r : (7*r + 7)]]
		logger.info(msg)
	logger.info('--------------')


def playMatches(player1, player2, EPISODES, logger, turns_until_tau0, memory = None, goes_first = 0):

    env = Game()
    scores = {player1.name:0, "drawn": 0, player2.name:0}
    sp_scores = {'sp':0, "drawn": 0, 'nsp':0}
    points = {player1.name:[], player2.name:[]}

    player1Starts = -1
    for e in range(EPISODES):

        logger.info('====================')
        logger.info('EPISODE %d OF %d', e+1, EPISODES)
        logger.info('====================')

        print (str(e+1) + ' ', end='')

        state = env.Reset()
        
        done = 0
        turn = 0
        player1.mcts = None
        player2.mcts = None

        if goes_first == 0:
            player1Starts = -player1Starts
            #random.randint(0,1) * 2 - 1
        else:
            player1Starts = goes_first

        if player1Starts == 1:
            players = {1:{"agent": player1, "name":player1.name}
                    , -1: {"agent": player2, "name":player2.name}
                    }
            logger.info(player1.name + ' plays as X')
        else:
            players = {1:{"agent": player2, "name":player2.name}
                    , -1: {"agent": player1, "name":player1.name}
                    }
            logger.info(player2.name + ' plays as X')
            logger.info('--------------')

        render(env, logger)
        print(env.GameState.ToString())

        ### Start single match
        while done == 0:
            turn = turn + 1
    
            #### Run the MCTS algo and return an action
            if turn < turns_until_tau0:
                action, pi, MCTS_value, NN_value = players[state.PlayerTurn]['agent'].act(state, 1)
            else:
                action, pi, MCTS_value, NN_value = players[state.PlayerTurn]['agent'].act(state, 0)

            if memory != None:
                ####Commit the move to memory
                memory.commit_stmemory(env.Identities(state, pi))


            logger.info('action: %d', int(action))
            for r in range(Game.SizeY):
                msg = ['----' if x == 0 else '{0:.2f}'.format(np.round(x,2)) for x in pi[Game.SizeX*r : (Game.SizeX*r + Game.SizeX)]]
                logger.info(msg)
            if MCTS_value: 
                logger.info('MCTS perceived value for %s: %f', GameState.Pieces[str(state.PlayerTurn)] ,np.round(MCTS_value,2))
            if NN_value:
                logger.info('NN perceived value for %s: %f', GameState.Pieces[str(state.PlayerTurn)] ,np.round(NN_value,2))
            logger.info('====================')
            ### Do the action
            env.Step(int(action))
            state = env.GameState
            value = env.GameState.Value
            done = env.GameState.Done

            render(env, logger)
            print(env.GameState.ToString())

            if done == 1: 
                if memory != None:
                    #### If the game is finished, assign the values correctly to the game moves
                    for move in memory.stmemory:
                        if move['playerTurn'] == state.PlayerTurn:
                            move['value'] = value
                        else:
                            move['value'] = -value
                         
                    memory.commit_ltmemory()
             
                if value == 1:
                    logger.info('%s WINS!', players[state.PlayerTurn]['name'])
                    scores[players[state.PlayerTurn]['name']] = scores[players[state.PlayerTurn]['name']] + 1
                    if state.PlayerTurn == 1: 
                        sp_scores['sp'] = sp_scores['sp'] + 1
                    else:
                        sp_scores['nsp'] = sp_scores['nsp'] + 1

                elif value == -1:
                    logger.info('%s WINS!', players[-state.PlayerTurn]['name'])
                    scores[players[-state.PlayerTurn]['name']] = scores[players[-state.PlayerTurn]['name']] + 1
               
                    if state.PlayerTurn == 1: 
                        sp_scores['nsp'] = sp_scores['nsp'] + 1
                    else:
                        sp_scores['sp'] = sp_scores['sp'] + 1

                else:
                    logger.info('DRAW...')
                    scores['drawn'] = scores['drawn'] + 1
                    sp_scores['drawn'] = sp_scores['drawn'] + 1

                points[players[state.PlayerTurn]['name']].append(-1)
                points[players[-state.PlayerTurn]['name']].append(1)
                logger.info(scores)
                print(scores)

    return (scores, memory, points, sp_scores)

