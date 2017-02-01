import clr
clr.AddReferenceByPartialName('UnityEngine')
import UnityEngine
import sys
sys.path.append(UnityEngine.Application.dataPath + '/../Python/Lib')

def print_message():
	UnityEngine.Debug.Log('Test message from Moderator.py!!!!')

if __name__ == '__main__':
    print_message()