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

# 壁情報
WALLS = ["1101", "1011", "1010", "1100", "1101",
		 "0001", "1010", "1010", "0100", "0101",
		 "0101", "1011", "1100", "0011", "0100",
		 "0011", "1000", "0100", "1001", "0110",
		 "1011", "0110", "0111", "0011", "1110"]