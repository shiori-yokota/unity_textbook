import clr
clr.AddReferenceByPartialName('UnityEngine')
import UnityEngine
import sys
sys.path.append(UnityEngine.Application.dataPath + '/../Python/Lib')
import datetime
def print_message():
    UnityEngine.Debug.Log('Test message from Moderator!!!!')
    UnityEngine.Debug.Log(datetime.datetime.today())
print_message()

# 定義
SIZE = 5

