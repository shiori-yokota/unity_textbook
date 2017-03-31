import clr
clr.AddReferenceByPartialName('UnityEngine')
import UnityEngine
import sys
sys.path.append(UnityEngine.Application.dataPath + '/../Python/Lib')
import random


# 初期化する
def init_list():
	Q = [0.0 for state in range(SIZE * SIZE)]
	# for state in range(SIZE * SIZE):
	# 	UnityEngine.Debug.Log('Q['+str(state)+'] : ' + str(Q[state]))
	return Q

def sampling(act, wall):
	newPRTCL = init_list()

	for state in range(SIZE * SIZE):
		numPRTCL = int(PRTCL[state])
		char_list = list(tmpWALLS[state])

		if int(char_list[act]) == 0:
			if act == 0:
				if state > 0:
					for num in range(numPRTCL):
						if random.uniform(0, 1) < TRANS:
							newPRTCL[state - SIZE] = newPRTCL[state - SIZE] + 1.0
						else:
							newPRTCL[state] = newPRTCL[state] + 1.0
			elif act == 1:
				if state != (4 and 9 and 14 and 19 and 24):
					for num in range(numPRTCL):
						if random.uniform(0, 1) < TRANS:
							newPRTCL[state + 1] += 1.0
						else:
							newPRTCL[state] += 1.0
			elif act == 2:
				if state < 20:
					for num in range(numPRTCL):
						if random.uniform(0, 1) < TRANS:
							newPRTCL[state + SIZE] += 1.0
						else:
							newPRTCL[state] += 1.0
			elif act == 3:
				if state != (0 and 5 and 10 and 15 and 20):
					for num in range(numPRTCL):
						if random.uniform(0, 1) < TRANS:
							newPRTCL[state - 1] += 1.0
						else:
							newPRTCL[state] += 1.0
		else:
			newPRTCL[state] = newPRTCL[state] + PRTCL[state]

	for state in range(SIZE * SIZE):
		PRTCL[state] = newPRTCL[state]

def calcSENSOR(wall):
	for state in range(SIZE * SIZE):
		# それぞれの地点のセンサ情報と観測したセンサ情報が一致する場合
		if tmpWALLS[state] == wall:	# 各状態の壁情報 0(north)0(east)0(south)0(west)
			SENSOR[state] = KANSOKU;
		# それぞれの地点のセンサ情報と観測したセンサ情報が一致しない場合
		else:
			SENSOR[state] = (1 - KANSOKU) / 15

# 各マスに存在する粒子の重さの合計
def update_weights():	
	for state in range(SIZE * SIZE):
		PRTCL_weight[state] = PRTCL[state] * SENSOR[state]

def resampling():
	normalization(PRTCL_weight)

	# 累積分布
	box = init_list()
	n = 0
	for state in range(SIZE * SIZE):
		box[n] = box[n - 1] + PRTCL_weight[state];
		n = n + 1
	
	# ランダム値を生成
	tmpNum = [0.0 for i in range(PA)]
	index = [0.0 for i in range(PA)]
	rnd = [0.0 for i in range(PA)]
	for num in range(PA):
		tmpNum[num] = random.uniform(0, 1)
		index[num] = 0;
		for temp in range(num):
			if tmpNum[temp] < tmpNum[num]:
				if index[num] <= index[temp]:
					index[num] = index[temp] + 1
			else: index[temp] = index[temp] + 1

	for i in range(PA):
		rnd[index[i]] = tmpNum[i]

	# ランダム値をカウント
	i = 0
	j = 0
	NumBox = init_list()
	while (i < PA):
		if rnd[i] < box[j]:
			NumBox[j] = NumBox[j] + 1
			i += 1
		else:
			j += 1

	# カウントした数を代入する
	n = 0;
	for state in range(SIZE * SIZE):
		PRTCL[state] = NumBox[n];
		n += 1
	
def normalization(g):
	total = 0.0
	for state in range(SIZE * SIZE):
		total = total + g[state]
	for state in range(SIZE * SIZE):
		PRTCL_weight[state] = g[state] / total

################ main ###############

if TimeCount == 0:	# TimeCountは送られてくる
	UnityEngine.Debug.Log('************')
	UnityEngine.Debug.Log('粒子の分布を初期化（均等に分布する）')
	PRTCL = [(float(PA) / (SIZE * SIZE)) for state in range(SIZE * SIZE)]
	
else:
	UnityEngine.Debug.Log('1) 粒子ごとに次状態を状態遷移確率を用いてサンプリング')
	sampling(ACTION,WALL)

	UnityEngine.Debug.Log('2) センサ情報の観測確率を計算')
	SENSOR = init_list()
	calcSENSOR(WALL)

	UnityEngine.Debug.Log('3) 粒子の重みづけ')
	PRTCL_weight = init_list()
	update_weights()

	UnityEngine.Debug.Log('4) リサンプリング')
	resampling()