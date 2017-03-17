import clr
clr.AddReferenceByPartialName('UnityEngine')
import UnityEngine
import sys
sys.path.append(UnityEngine.Application.dataPath + '/../Python/Lib')
import random


# 初期化する
def init_SONZAI():
	Q = [0.0 for state in range(SIZE * SIZE)]
	# for state in range(SIZE * SIZE):
	# 	UnityEngine.Debug.Log('Q['+str(state)+'] : ' + str(Q[state]))
	return Q

def calcSONZAI(act):
	for state in range(SIZE * SIZE):
		SONZAI[state] = 0.1;

def calcSENSOR():
	tmpwall = 1;
	wall = 2;
	for state in range(SIZE * SIZE):
		# それぞれの地点のセンサ情報と観測したセンサ情報が一致する場合
		if tmpwall == wall:
			SENSOR[state] = KANSOKU;
		# それぞれの地点のセンサ情報と観測したセンサ情報が一致しない場合
		else:
			SENSOR[state] = (1 - KANSOKU) / 15

def information_integration():
	for state in range(SIZE * SIZE):
		G[state] = SONZAI[state] * SENSOR[state]

def normalization(g):
	total = 0.0
	for state in range(SIZE * SIZE):
		total += g[state]
	for state in range(SIZE * SIZE):
		SONZAI[state] = g[state] / total


################ main ###############

if TimeCount == 0:	# TimeCountは送られてくる
	# F_0(s_0)を初期化する
	UnityEngine.Debug.Log('************')
	SONZAI = init_SONZAI()

	UnityEngine.Debug.Log('無情報')
	preSONZAI = [(1.0 / (SIZE * SIZE)) for state in range(SIZE * SIZE)]
else:
	UnityEngine.Debug.Log('1) ロボットが進んだので存在確率を計算する')
	calcSONZAI(ACTION);	# ACTIONは送られてくる

	UnityEngine.Debug.Log('2) センサ情報を計算する')
	SENSOR = init_SONZAI()
	calcSENSOR()

	UnityEngine.Debug.Log('3) 移動と観測情報の統合')
	G = init_SONZAI()
	information_integration()

	UnityEngine.Debug.Log('4) 正規化')
	normalization(G)

	preSONZAI = SONZAI