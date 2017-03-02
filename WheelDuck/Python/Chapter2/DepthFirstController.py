import clr
clr.AddReferenceByPartialName('UnityEngine')
import UnityEngine
import sys
sys.path.append(UnityEngine.Application.dataPath + '/../Python/Lib')

list = ["S3", "S4", "S1", "S4", "S6", "G", "S6", "S2", "S6", "S4", "S3", "S7", "S8", "S5", "S8", "S10", "S8", "S7", "S9"]

#while OPENLIST:
UnityEngine.Debug.Log('OPENLIST : ' + str(OPENLIST))
UnityEngine.Debug.Log('CLOSEDLIST : ' + str(CLOSEDLIST))

#while OPENLIST:
#	label = OPENLIST.pop(0)
#	if label == GOAL:
#		CLOSEDLIST.append(label)
#		# 解が発見されて終了
#		UnityEngine.Debug.Log('Finish python code!!')

#	if label not in CLOSEDLIST:
#		CLOSEDLIST.append(label)
#		OPENLIST = list + OPENLIST

CLOSEDLIST = OPENLIST + list

UnityEngine.Debug.Log('OPENLIST : ' + str(OPENLIST))
UnityEngine.Debug.Log('CLOSEDLIST : ' + str(CLOSEDLIST))