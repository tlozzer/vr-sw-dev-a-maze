# A Maze
Starter project for the Udacity [VR Developer Nanodegree](http://udacity.com/vr) program.

- Course: VR Software Development
- Project: A Maze

This is a game where you need to navigate through a maze, find a key lost somewhere in it, find the maze exit and open the temple's door.
If you find the maze exit before picking the key, you will not be able to open the temple's door. In that case, you should return into the maze and find the key.

- Maze size can be customized in Unity Editor editing MazeGenerator script parameters under Maze GameObject. Number of Rows and Columns are constrained between 5 and 15. The fewer the rows and columns, the easier is to find the maze exit.
- Maze coins quantity also can be customized in MazeGenerator script parameters under Maze GameObject. If the parameter value is 6, that means that there will be 1 key and 5 coins in the maze. If 21, there will be 1 key and 20 coins. The minimum value is 6 and the maximum is rows times columns.
- The Maze is auto-generated on game start. The code was based on this page's algorithm: [Labirinto - Introdução a Programação em C com Jogos 2D](https://sites.google.com/a/liesenberg.biz/cjogos/home/trabalhos-praticos/atividades-programadas/labirinto), written in portuguese.

### Main Scene
- 

### Versions Used
- [Unity LTS Release 2017.4.24f1](https://unity3d.com/unity/qa/lts-releases?version=2017.4)
- [GVR SDK for Unity v1.170.0](https://github.com/googlevr/gvr-unity-sdk/releases/tag/v1.100.1)

### Target Platform
- Android
