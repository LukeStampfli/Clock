# Clock
A completly accurate timer with performance tracking for multiplayer game servers

How to use:\
Add the script to your project and create a clock:
```csharp
Clock clock = new Clock(40, true, 10000)
```
This creates a clock with a tickrate of 40 ticks per second. (and a delta time of 25ms).\
It will write a log to the console every 10000 ticks.(in this case every 250 seconds)
The log contains 3 values:
- The current Tick
- The summation of all the delta times since the last Tick(This should be < lograte*delta time, or your server uses too much resources)
- the max delta time sine the last Tick(Idealy this is < delta time of the clock, if it is much greater than the delta time players might feel lag spikes)

Subscribe to the clock:
```csharp
clock.Tick += YourFunction
// YourFunction can be any Void function.
```
