import clr
clr.AddReferenceByPartialName('UnityEngine')
import UnityEngine
import sys
sys.path.append(UnityEngine.Application.dataPath + '/../Python/Lib')

# 迷路のサイズ
SIZE = 5
# Goal位置を設定
GOAL_COL = 4
GOAL_ROW = 4
# 報酬
GOAL_REWARD = 100.0		# ゴールにたどりついた場合
HIT_WALL_PENALTY = -10.0	# 壁にぶつかった場合
ONE_STEP_PENALTY = -1.0	# 壁にぶつからずに1マス進んだ場合
# epsilon-greedy法のパラメータ
EPSILON = 0.3
# 割引率
GAMMA = 0.9
# 学習率
BETA = 0.1
