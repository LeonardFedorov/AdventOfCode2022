# Advent Of Code 2022

## Introduction

This project contains all of my solutions for the Advent of Code 2022 puzzle set (www.adventofcode.com) combined into a single project alongside my personalised input data. The entire project has been written in F# and is targeting a functional first approach, trying to minimise mutable values or side effects.

This code is all solely my own work, and you can reference or use it to whatever extent it may be of use for you to do so. This is done entirely at your own risk as I make no claims this code is best practice, efficient, or stable outside of the limited scope of calculating my AoC answers.

## Structure

The executable entry point is in the MainProgram.fs file. This file contains the user input loop which allows the User to specify which days results are to be calculated and displayed. The calculations for each day are contained in separate source files, one for each day.

The input data is kept in the repository, with the pathing to this data handled by a routine in the MainProgram file. It is currently set up relative to the project folder so the project should build and run in Visual Studio fine.

## Notes on the Puzzles

(Obviously, some spoilers for the puzzle solutions)

<details>
	<summary>Day 1</summary>

A straight forward puzzle, nothing much to say really.

</details>

<details>
	<summary>Day 2</summary>

Not too difficult in principle, but I initially ended up going down a path of explicitly pattern matching all cases which can be seen in the first commit of day 2. However, I subsequently realised a more subtle way of expressing the win relationsihps by setting the items up as an enum and then using an array with the indicies aligned to the enum so that offsetting left or right in the array would be able to determine the result of the game. 

</details>

<details>
	<summary>Day 3</summary>

A fairly straight forward string searching exercise. After completing part 2, I refactored part 1 to use the same searching code as part 2 to tidy up the solution a bit. Some char value hacking which is always entertaining to read.

</details>

<details>
	<summary>Day 4</summary>

 Very easy day, just hard typed the logical comparisons so not at all scalable to rows with more than 2 ranges. Use of array argument pattern matching makes the expression very concise though - albeit at the expense of generating a compiler warning due to the potential for other patterns (which obviously don't arise because the input data is tightly controlled)

</details>

<details>
	<summary>Day 5</summary>

An interesting problem to parse the data and find a suitable structure for manipulating it. I settled on using an array of lists (with the top of each pile being the head of the list), as the list structure makes it fairly easy to "move" the items around between the lists. Initially, there was an issue with the initial box state being mutated by the part 1 process, leading to the part 2 process acting on the post-part 1 state instead of the initial state. Changing the initial box state to a 1 variable function rather than being a value fixed by, I assume, forcing the evaluation of it each time it is called. There is probably a cleaner way of handling this (evaluate the initial state once from the input and then just create copies of it?) but I didn't spend the time to find it as I try to avoid mutable constructs anyway.

</details>

<details>
	<summary>Day 6</summary>

The only trick from a functional point of view was figuring out how to perform the iteration cleanly. I settled on generating the array of candidate indicies (i.e. markerlength up to string length) and then performing Array.find over that to locate the first one. Since I had parameterised marker length in the course of the part 1 solution, part 2 was solved instantly.

</details>

<details>
	<summary>Day 7</summary>

Not attempted yet.

</details>

<details>
	<summary>Day 8</summary>

Not attempted yet.

</details>

<details>
	<summary>Day 9</summary>

Not attempted yet.

</details>

<details>
	<summary>Day 10</summary>

Not attempted yet.

</details>

<details>
	<summary>Day 11</summary>

Not attempted yet.

</details>

<details>
	<summary>Day 12</summary>

Not attempted yet.

</details>

<details>
	<summary>Day 13</summary>

Not attempted yet.

</details>

<details>
	<summary>Day 14</summary>

Not attempted yet.

</details>

<details>
	<summary>Day 15</summary>

Not attempted yet. 

</details>

<details>
	<summary>Day 16</summary>

Not attempted yet.

</details>

<details>
	<summary>Day 17</summary>

Not attempted yet.

</details>

<details>
	<summary>Day 18</summary>

Not attempted yet.

</details>

<details>
	<summary>Day 19</summary>

Not attempted yet.

</details>

<details>
	<summary>Day 20</summary>

Not attempted yet.

</details>

<details>
	<summary>Day 21</summary>

Not attempted yet.

</details>

<details>
	<summary>Day 22</summary>

Not attempted yet.

</details>

<details>
	<summary>Day 23</summary>

Not attempted yet.

</details>

<details>
	<summary>Day 24</summary>

Not attempted yet.

</details>

<details>
	<summary>Day 25</summary>

Not attempted yet.

</details>