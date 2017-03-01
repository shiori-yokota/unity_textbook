import clr
clr.AddReferenceByPartialName('UnityEngine')
import UnityEngine
import sys
sys.path.append(UnityEngine.Application.dataPath + '/../Python/Lib')

list = ["S3", "S4", "S7"]

#while OPENLIST:
UnityEngine.Debug.Log('OPENLIST : ' + str(OPENLIST))
UnityEngine.Debug.Log('CLOSEDLIST : ' + str(CLOSEDLIST))

while OPENLIST:
	label = OPENLIST.pop(0)
	if label == GOAL:
		CLOSEDLIST.append(label)
		# 解が発見されて終了
		UnityEngine.Debug.Log('Finish python code!!')

	if label not in CLOSEDLIST:
		CLOSEDLIST.append(label)
		OPENLIST = list + OPENLIST

UnityEngine.Debug.Log('OPENLIST : ' + str(OPENLIST))
UnityEngine.Debug.Log('CLOSEDLIST : ' + str(CLOSEDLIST))