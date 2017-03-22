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

################ main ###############

if TimeCount == 0:	# TimeCountは送られてくる
	UnityEngine.Debug.Log('************')
	UnityEngine.Debug.Log('粒子の分布を初期化（均等に分布する）')
	PRTCL = [(float(PA) / (SIZE * SIZE)) for state in range(SIZE * SIZE)]
	
else:
	UnityEngine.Debug.Log('1) 粒子ごとに次状態を状態遷移確率を用いてサンプリング')

	UnityEngine.Debug.Log('2) センサ情報の観測確率を計算')

	UnityEngine.Debug.Log('3) 粒子の重みづけ')

	UnityEngine.Debug.Log('4) リサンプリング')
