import clr
clr.AddReferenceByPartialName('UnityEngine')
import UnityEngine
import sys
sys.path.append(UnityEngine.Application.dataPath + '/../Python/Lib')

def init_list(list):


state = [S, S1, S2, S3, S4, S5, S6, S7, S8, S9, S10, G]
act = [[S, S3], [S1, S4], [S2, S6], [S3, S4], [S3, S7], [S4, S6], [S5, S8], [S6, G], [S7, S8], [S7, S9], [S8, S10]]
openlist = []
closedlist = []
 
init_list(openlist)
