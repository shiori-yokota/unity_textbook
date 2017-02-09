import clr
clr.AddReferenceByPartialName('UnityEngine')
import UnityEngine
import sys
sys.path.append(UnityEngine.Application.dataPath + '/../Python/Lib')
import random
filename = UnityEngine.Application.dataPath + '/../Python/Chapter7/qvalues.txt'


# Q値を初期化する
def init_qvalues():
	Q = [[ 0.0 for num_actions in range(4)] for state in range(SIZE * SIZE)]
	#for state in range(SIZE * SIZE):
	#		for act in range(4):
	#			UnityEngine.Debug.Log('Q['+str(state)+']['+str(act)+'] : ' + str(Q[state][act]))
	return Q

# ロボットのstateを設定する
def mazeNum(row, col):
	if row == 0:
		state = col
	else:
		state = SIZE * row + col
	return state

# 行動を選択する
def eGreedy(Q, state):
	rndm_val = random.randint(0, 100)
	best_val = -100000.0
	curr_val = -100000.0
	tmp_act = []
	action = -1
	
	if rndm_val > EPSILON * 100: # (1 - epsilon)の確率 ランダムにactを選択
		action = random.randint(0, 3)
	else: # epsilonの確率 Q値が最大となるようなactionを選択
		for act in range(4):
			curr_val = float(Q[state][act])
			if curr_val > best_val:
				best_val = curr_val
				action = act
				tmp_act.append(action)
			elif curr_val == best_val:
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
			UnityEngine.Debug.Log('[↑: 0]')
		elif action == 1:
			UnityEngine.Debug.Log('[→: 1]')
		elif action == 2:
			UnityEngine.Debug.Log('[↓: 2]')
		elif action == 3:
			UnityEngine.Debug.Log('[←: 3]')
	return action

# Q値を更新する
def update_qvalue(Q, old_state, new_state, act, reward):
	best_new_qval = best_qvalue(Q, new_state)
	qval = Q[old_state][act]
	Q[old_state][act] = (1.0 - BETA) * float(qval) + BETA * (reward + GAMMA * float(best_new_qval))
	UnityEngine.Debug.Log('Q['+str(old_state)+']['+str(act)+'] : ' + str(Q[old_state][act]))
	return Q

def best_qvalue(Q, state):
	best_val = -1000000.0
	for i in range(4):
		if Q[state][i] > best_val:
			best_val = Q[state][i]
	return best_val;

# Q値を記録する
def writeQvalue(Q):
	output = open(filename, 'w')
	for state in range(SIZE * SIZE):
		for action in range(4):
			output.write('qvalue['+str(state)+']['+str(action)+']:'+str(Q[state][action]))
			output.write('\n')
	output.close()

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

# Q値を初期化する
if CONTINUE != True:
	UnityEngine.Debug.Log('************')
	UnityEngine.Debug.Log('EPISODE : ' + str(EPISODE))
	qvalue = init_qvalues()
	if INIT != True:
		qvalue = readQvalue(qvalue)
	# ロボットのstateを設定する
	state = mazeNum(ROW, COL)
	# Q値の保存
	writeQvalue(qvalue)
	# 行動を選択する -> 移動
	ACT = eGreedy(qvalue, state)
else:
	# 報酬が返ってくる
	UnityEngine.Debug.Log('Reward : ' + str(REWARD))
	qvalue = init_qvalues()
	qvalue = readQvalue(qvalue)
	#for state in range(SIZE * SIZE):
	#	for act in range(4):
	#		UnityEngine.Debug.Log('qvalue['+str(state)+']['+str(act)+'] : ' + str(qvalue[state][act]))
	old_state = mazeNum(OLD_ROW, OLD_COL)
	new_state = mazeNum(NEW_ROW, NEW_COL)
	qvalue = update_qvalue(qvalue, old_state, new_state, ACT, REWARD)
	UnityEngine.Debug.Log('************')
	# Q値の保存
	writeQvalue(qvalue)
	# 行動を選択する -> 移動
	ACT = eGreedy(qvalue, new_state)
