# HexLib
An independent C#/.NET library containing logic to **generate grids of hexagonal axial coordinates**.

## Why?
Created mostly as a bit of fun to expand on what I had already created for my Hexagonal Strategy Game (working title: **Bedrock**).

## Uses
### HexCoordinates
Talking atomically, this is the smallest object class, which contains axial coordinate values (Q, R). There is a built-in method to convert to **cube coordinates** if that's your cup of tea.
### HexGrid
This is a static class with factory methods that generate grids of **HexCoordinates**. There are currently 3 shapes - **hexagon**, **triangle** and **rectangle**.
### HexPathfinder
Uses the **A\* Algorithm** to pathfind the most efficient path from one HexCoordinate object to another.

## Suggestions?
Let me know if you have any ideas or suggestions. This is a fun little side project and I think I'll keep working on it for the foreseeable future.
