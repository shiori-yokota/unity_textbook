import clr
clr.AddReferenceByPartialName('UnityEngine')
import UnityEngine
import sys
sys.path.append(UnityEngine.Application.dataPath + '/../Python/Lib')
def print_message():
    UnityEngine.Debug.Log('Test message from Moderator.py!!!!')

# 迷路のサイズ
SIZE = 5
# Goal位置を設定
GOAL_COL = 4
GOAL_ROW = 4
# 報酬
GOAL_REWARD = 100.0		# ゴールにたどりついた場合
HIT_WALL_PENALTY = -10.0	# 壁にぶつかった場合
ONE_STEP_PENALTY = -1.0	# 壁にぶつからずに1マス進んだ場合

## ロボットの初期位置設定 ##
#import numpy
#def InitRobotPosition():
#    row_num = numpy.random.randint(0, SIZE - 1)
#    col_num = numpy.random.randint(0, SIZE - 1)

#    ### 初期位置がGOALだったらもう一度初期位置を決める ###
#    if row_num == GOAL_ROW and col_num == GOAL_COL:
#        InitRobotPosition()
#    else:
#        setRobotPosition = True
#        row = row_num
#        col = col_num

if __name__ == '__main__':
    print_message()
    #InitRobotPosition()