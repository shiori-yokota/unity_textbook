import clr
clr.AddReferenceByPartialName('UnityEngine')
import UnityEngine
import sys
sys.path.append(UnityEngine.Application.dataPath + '/../Python/Lib')
import random

# Q値を初期化する
def init_qvalues():
	Q = [[[ 0.0 for num_actions in range(4)] for col in range(SIZE)] for row in range(SIZE)]
	#for row in range(SIZE):
	#	for col in range(SIZE):
	#		for act in range(4):
	#			UnityEngine.Debug.Log('Q['+str(row)+']['+str(col)+']['+str(act)+'] : ' + str(Q[row][col][act]))
	return Q

# 行動を選択する
def eGreedy(Q):
	rndm_val = random.randint(0, 100)
	best_val = -100000.0
	curr_val = -100000.0
	tmp_act = []
	
	if rndm_val > EPSILON * 100: # (1 - epsilon)の確率 ランダムにactを選択
		action = random.randint(0, 3)
	else: # epsilonの確率 Q値が最大となるようなactionを選択
		for act in range(4):
			UnityEngine.Debug.Log('Q['+str(ROW)+']['+str(COL)+']['+str(act)+'] : ' + str(Q[ROW][COL][act]))
			curr_val = Q[ROW][COL][act]
			if curr_val > best_val:
				best_val = curr_val
				action = act
				tmp_act.append(action)
			elif curr_val == best_val:
				action = act
				tmp_act.append(action)
				if len(tmp_act) > 0:
					tmpNum = random.randint(0, len(tmp_act))
					action = tmp_act[tmpNum]

	if action < 0 or action > 3:
		UnityEngine.Debug.Log('action error')
	if action == 0:
		UnityEngine.Debug.Log('[↑: 0]')
	elif action == 1:
		UnityEngine.Debug.Log('[→: 1]')
	elif action == 2:
		UnityEngine.Debug.Log('[↓: 2]')
	elif action == 3:
		UnityEngine.Debug.Log('[←: 3]')
	return action

def QLearning():
	# Q値を初期化する
	qvalue = init_qvalues()
	UnityEngine.Debug.Log('************')
	UnityEngine.Debug.Log('EPISODE : ' + str(EPISODE))
	# 行動を選択する
	action = eGreedy(qvalue)

QLearning()