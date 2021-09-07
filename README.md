
# DBClib

CAN database file parsing class library for C#

# Usage

Add DBClib class into your solution. You may use all functions related DBC parsing as seen below.

#### GetSignalNames(string file, string MSGid)
#### GetMessageNames(string file)
#### GetMessageIDs(string file)
#### GetMin(string file, string signal)
#### GetMax(string file, string signal)
#### GetUnit(string file, string signal)
#### GetFactor(string file, string signal)
#### GetOffset(string file, string signal)
#### GetStartBit(string file, string signal)
#### GetEndBit(string file, string signal)
#### GetLength(string file, string signal)
#### GetCycleTime(string file, string MSGid)
#### GetValue(string payload, string file, string signal)

These functions are compatible with Standart CAN dbc and J1939 dbc files.

|                |Standart CAN DBC               |J1939 DBC                    |
|----------------|-------------------------------|-----------------------------|
|Signal Functions|Supported             |Supported            |
|Message Functions|Supported            |Supported            |
|GetValue()      |Supported             |Supported            |

# TODOlist

#### 1. GetCycleTime and GetValue functions performance improvement

According to unit test results, these functions must be reviewed.

![enter image description here](https://i.imgur.com/sVDwb1G.png)

#### 2. Bugfix for in case of same signal used on different MSGs.
#### 3. Development of final test GUI.

Please visit project page for detailed information about development process. 
https://github.com/users/alperunlu/projects/1
