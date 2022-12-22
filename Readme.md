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

Ended up taking ages to do this as I got sidetracked on the day and things just took over. Life was much easier after I threw away the recursive data structure idea and just built it as a straight array of strings - sometimes simple is just better! The computation of all the directory sizes could probably be made much more efficient through a function memorisation or deliberate sequencing, but the runtime was instant enough it didn't seem worth the bother to attempt to optimise.

</details>

<details>
	<summary>Day 8</summary>

Not challenging conceptually, though was a bit odd that parts 1 and 2 were basically unrelated. Part 1 solved through an efficient (thoughly wholly imperative construction), whereas part 2 was ultimately more functional in nature. Using a 2D array proved very powerful for the slicing functionality which made the computation quite easy to express.

</details>

<details>
	<summary>Day 9</summary>

Part 2 required a significant refactor of the data structure, but all the core logic worked perfectly after the transition. Although the inner construction uses a mutable list to build up the rope position, the overall construct is a purely functional recursive loop construct.

</details>

<details>
	<summary>Day 10</summary>

Part 2 took some effort to get the index aligment correct (it would also have helped if I had read the sprite processing rules precisely correctly first time as well!). Again, part 1 was achieved using a mutable array which allowed for easily handling the variable length steps.

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

An interesting one, with a pleasing re-use of the part 1 code to solve part 2. Part 1 was adding the points to the shape one by one, and observing that the surface area of the shape increased by 6 - 2 * number of cubes adjacent to the new one. Part 2 was then done by creating a 3 dimensional array representing the bounding box of the shape from part 1, and then doing a fill from the bottom left corner to identify the outside points (and therefore the inside points as the complement). The part 1 code was then used to work out the surface area of these interior points so that the solution was the part 1 answer minus the surface area of the interior holes.

</details>

<details>
	<summary>Day 19</summary>

Assumptions:
	Never pass on building geo robot if available
	Never build more robots producing a given resource than cost of any given robot in that resource (since only one robot can be built per turn)
	If a robot was available to be built, but passed on, don't consider building it again until some other robot is built.
	If all "available" (being all robots whose source materials are all currently in production) robots can be afforded, something should be built.
	Any bot that is going to be built ought to be built as soon as resources are available, unless something else is being built first (i.e. no lollygagging)

</details>

<details>
	<summary>Day 20</summary>

Took a while to fully get the hang of this one, as the wrap-around impact had a few subtleties. The key realisation is that because the object is being moved to between two other objects (including the two it is between presently), there are only Array Length minus 1 possible places (another way of seeing this is to consider that being moved into the first position or the last position of the array is identical as far as the order goes if everything else is done right) to consider. Doing the offset arithmetic mod Array Length minus 1 simplified the logic and removed the need for kinda janky special case arithmetic.
Part 2 offered nothing special - just jack up the numbers to check the reader was aware of 64 bit variables and then iterate the process several times.
</details>

<details>
	<summary>Day 21</summary>

Part 1 was fairly straight forward and didn't pose much challenge, but part 2 proved trickier. The concept of building the calculation stack from the humn point up to the root was fairly easy to implement, but defining the monkey as a discrinated union made the code very syntactically cumbersome. A different way of representing the monkeys would probably have helped here. Another thing was I had stored the monkey's function inside them, but then couldn't see a way to match against it in part 2 so I hastily tacked on the operator symbol as an extra parameter. Overall, not massively happy with how this turned out, but at least it works.

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