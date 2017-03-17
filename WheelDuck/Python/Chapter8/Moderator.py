import clr
clr.AddReferenceByPartialName('UnityEngine')
import UnityEngine
import sys
sys.path.append(UnityEngine.Application.dataPath + '/../Python/Lib')

# 迷路のサイズ
SIZE = 5

# 状態遷移確率
TRANS = 0.8

# 観測確率
KANSOKU = 0.7
