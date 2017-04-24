import clr
clr.AddReferenceByPartialName('UnityEngine')
import UnityEngine
import sys
sys.path.append(UnityEngine.Application.dataPath + '/../Python/Lib')

########## Example ##########

Sequence_list = ["S3", "S4", "S7"]

UnityEngine.Debug.Log('OPENLIST : ' + str(OPENLIST))
UnityEngine.Debug.Log('CLOSEDLIST : ' + str(CLOSEDLIST))

queue = OPENLIST

while queue:
	label = queue.pop(0)
	if label == GOAL:
		CLOSEDLIST.append(label)
		# 解が発見されて終了
		UnityEngine.Debug.Log('Finish Python code!!')

	if label not in CLOSEDLIST:
		CLOSEDLIST.append(label)
		queue += Sequence_list

#############################

UnityEngine.Debug.Log('OPENLIST : ' + str(OPENLIST))
UnityEngine.Debug.Log('CLOSEDLIST : ' + str(CLOSEDLIST))