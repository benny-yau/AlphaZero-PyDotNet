import numpy as np
from collections import deque

import config

class Memory:
	def __init__(self, MEMORY_SIZE):
		self.MEMORY_SIZE = config.MEMORY_SIZE
		self.ltmemory = deque(maxlen=config.MEMORY_SIZE)
		self.stmemory = deque(maxlen=config.MEMORY_SIZE)

	def commit_stmemory(self, identities):
		for r in identities:
			self.stmemory.append({
				'board': np.fromiter(r[0].Board, dtype = int)
				, 'state': r[0]
				, 'id': r[0].Id
				, 'AV': np.fromiter(r[1], dtype = int)
				, 'playerTurn': r[0].PlayerTurn
				})

	def commit_ltmemory(self):
		for i in self.stmemory:
			self.ltmemory.append(i)
		self.clear_stmemory()

	def clear_stmemory(self):
		self.stmemory = deque(maxlen=config.MEMORY_SIZE)
		