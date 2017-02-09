import clr
clr.AddReferenceByPartialName('UnityEngine')
import UnityEngine
import sys
sys.path.append(UnityEngine.Application.dataPath + '/../Python/Lib')
import random
filename = UnityEngine.Application.dataPath + '/../Python/Chapter7/qvalues.txt'

# ロボットのstateを設定する
def mazeNum(row, col):
	if row == 0:
		state = col
	else:
		state = SIZE * row + col
	return state

# Q値を初期化する
def init_qvalues():
	Q = [[ 0.0 for num_actions in range(4)] for state in range(SIZE * SIZE)]
	#for state in range(SIZE * SIZE):
	#		for act in range(4):
	#			UnityEngine.Debug.Log('Q['+str(state)+']['+str(act)+'] : ' + str(Q[state][act]))
	return Q

# 記録したQ値を読み込む
def readQvalue(Q):
	input = open(filename, 'r')
	lines = input.readlines()
	value = []
	for line in lines:
		line = line.replace('\n','')
		line = line.replace('\r','')
		itemList = line.split(':')[1]
		value.append(itemList)
	input.close()
	s1 = 0
	s2 = 4
	for i in range(SIZE * SIZE):
		a = value[s1:s2]
		for index in range(len(a)):
			Q[i][index] = a[index]
		s1 += 4
		s2 += 4
	return Q

# 
def best_value_action(state, Q):
	best_value = -1000000.0
	action = -1
	tmp_act = []
	for act in range(4):
		UnityEngine.Debug.Log('Q['+str(state)+']['+str(act)+'] : ' + str(Q[state][act]))
		if float(Q[state][act]) > best_value:
			best_value = float(Q[state][act])
			UnityEngine.Debug.Log('best_value : ' + str(best_value))
			action = act
			UnityEngine.Debug.Log('action : ' + str(action))
			tmp_act.append(action)
		elif Q[state][act] == best_value:
			action = act
			tmp_act.append(action)
			if len(tmp_act) > 0:
				index = len(tmp_act) - 1
				tmpNum = random.randint(0, index)
				action = tmp_act[tmpNum]

	if action < 0 or action > 3:
		UnityEngine.Debug.Log('action error')
	else:
		if action == 0:
			UnityEngine.Debug.Log('[N: 0]')
		elif action == 1:
			UnityEngine.Debug.Log('[E: 1]')
		elif action == 2:
			UnityEngine.Debug.Log('[S: 2]')
		elif action == 3:
			UnityEngine.Debug.Log('[W: 3]')
	return action

qvalue = init_qvalues()
qvalue = readQvalue(qvalue)
UnityEngine.Debug.Log('ROW , COL : ' + str(ROW) + str(COL))
state = mazeNum(ROW, COL)
ACT = best_value_action(state, qvalue)
