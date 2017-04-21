import clr
clr.AddReferenceByPartialName('UnityEngine')
import UnityEngine
import sys
sys.path.append(UnityEngine.Application.dataPath + '/../Python/Lib')

########## Example ##########

Sequence_list = ["S3", "S4", "S1", "S6", "G", "S2", "S7", "S8", "S5", "S10","S9"]

UnityEngine.Debug.Log('OPENLIST : ' + str(OPENLIST))
UnityEngine.Debug.Log('CLOSEDLIST : ' + str(CLOSEDLIST))

stack = OPENLIST

while stack:
	label = stack.pop(0)
	UnityEngine.Debug.Log('label : ' + str(label))
	if label == GOAL:
		CLOSEDLIST.append(label)
		# 解が発見されて終了
		UnityEngine.Debug.Log('Finish python code!!')

	if label not in CLOSEDLIST:
		CLOSEDLIST.append(label)
		stack = Sequence_list

#############################

UnityEngine.Debug.Log('OPENLIST : ' + str(OPENLIST))
UnityEngine.Debug.Log('CLOSEDLIST : ' + str(CLOSEDLIST))