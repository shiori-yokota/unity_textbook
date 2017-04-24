import clr
clr.AddReferenceByPartialName('UnityEngine')
import UnityEngine
import sys
sys.path.append(UnityEngine.Application.dataPath + '/../Python/Lib')

########## Example ###########

Property_list = []
Property_list.append(["S1", 8])
Property_list.append(["S2", 5])
Property_list.append(["S3", 8])
Property_list.append(["S4", 5])
Property_list.append(["S5", 6])
Property_list.append(["S6", 3])
Property_list.append(["S7", 5])
Property_list.append(["S8", 4])
Property_list.append(["S9", 5])
Property_list.append(["S10", 3])
Property_list.append(["G", 0])

UnityEngine.Debug.Log('OPENLIST : ' + str(OPENLIST))
UnityEngine.Debug.Log('CLOSEDLIST : ' + str(CLOSEDLIST))



##############################

# コスト付きの状態集合から状態集合だけを取り出す処理
# flat_list = []
# for e in Property_list:
#	flat_list.extend(e)
# list_str = flat_list[0::2]
# CLOSEDLIST = OPENLIST + list_str

CLOSEDLIST = ["S", "S3", "S4", "S6", "G"]

UnityEngine.Debug.Log('OPENLIST : ' + str(OPENLIST))
UnityEngine.Debug.Log('CLOSEDLIST : ' + str(CLOSEDLIST))