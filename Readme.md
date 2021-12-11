# Advent Of Code 2021

## Introduction

This project contains all of my solutions for the Advent of Code 2021 puzzle set (www.adventofcode.com) combined into a single project alongside my personalised input data. The entire project has been written in F# and is targeting a purely functional approach, with no use of mutable values or side effects (aside from console I/O with the User).

This code is all solely my own work, and you can reference or use it to whatever extent it may be of use for you to do so. 

## Structure

The executable entry point is in the MainProgram.fs file. This file contains the user input loop which allows the User to specify which days results are to be calculated and displayed. The calculations for each day are contained in separate source files, one for each day.

The input data is kept in the repository, with the pathing to this data handled by a routine in the MainProgram file. It is currently set up relative to the project folder so the project should build and run in Visual Studio fine, but a more robust solution may be implemented later.

## Notes on the Puzzles

(Obviously, spoilers for the puzzle solutions)

<details>
	<summary>Day 1</summary>

A simple opening puzzle. The key observation which greatly simplifies the solution is to realise that if s_n(i) and s_n(i+1) are the sum of the n elements starting from index i and i+1 respectively, then s_n(i) < s_n(i+1) iff element(i) < element(i + n). This allows for variable length sums to be omitted entirely and the result array to build rapidly from single comparisons. 
</details>

<details>
	<summary>Day 2</summary>

This puzzle had some finesse in getting the update step formulae correct and tracking the parameters, but otherwise unremarkable algorithmically. The input instructions, having been formatted into an array with one instruction per element, are then folded through with the sub state held in the accumulator as a tuple of the relevant values. The only difference between parts 1 and 2 is the update function in the accumulator performing different calculations.

</details>

<details>
	<summary>Day 3</summary>

The first puzzle with some weight to it. Part 1 was fairly straight forward but part 2 required extra work around building the iterator. An optimisation here for part 2 would be to split out the process of evaluating the most common char in a given column from the function that calculates it for all columns as this since part 2 only uses them one at a time and they need recomputing each time anyway. However, given the execution speed is already unnoticeable with the full input set, this has not been done.

</details>

<details>
	<summary>Day 4</summary>

This puzzle, while not as hard as day 3, did pose some challenges, particularly around wrangling the input data in the first instance. I eventually settled on using the built in 2D arrays to store the bingo cards as this made it easier to manage the boolean tick array later. The array slicing notation proved very useful for doing the win tests and makes this part of the code quite elegant.

The code would run faster if the boolean tick array was mutated each iteration rather than being rebuilt, but in the interests of pure functionaltiy I have left this as is.

</details>

<details>
	<summary>Day 5</summary>

Day 5 is the first problem that takes a non-negligable amount of time to process. It also has been written so that it only outputs the part 2 solution and no longer produces the part 1 answer. The overhead arises from the fact that the rebuild of the array to update for each new line is being done over a c.1000x1000 so has a far greater impact than the 5x5 bingo cards in Day 4.

That inefficiency aside, the logic for identifying points on the lines was aided greatly by pre-processing the lines during data import to ensure they always ran left to right. Although the line test logic is explicitly split out into 4 separate cases, on reflection there is a lot of similarity so condensing it might be possible.

</details>

<details>
	<summary>Day 6</summary>

My first theorised solution was to build an array with 1 entry per fish to track each fish's reproduction timer and extend the array each step. Before beginning on the problem, I then realised that the fish being homogeneous meant that it sufficed to track how many were at each point in the cycle and therefore track a fixed length array of the cohorts. This turned out to be a good move as the final number of fish became extremely large.

In part 2, the number became so large it became necessary to move all the calculations to 64 bit integers to prevent overflow, but otherwise the algorithm was identical.

</details>

<details>
	<summary>Day 7</summary>

Day 7 was an easy one. The zero sum nature of the shift totals in the first part made it clear to see by an informal inductive argument that the median of the data set would always be the answer. The second part was not quite so receptive to analytic examination. It appears that answer is near the mean, but it's not exact. Given the brute force computation (accelerated by using the triangle number formula to get the fuel consumption) executes imperceptibly fast, I didn't bother to examine in more detail.

</details>

<details>
	<summary>Day 8</summary>

Day 8 was the hardest so far, eeking out even Day 3. The part 1 of the puzzle was very straight forward, but the looming requirement of part 2 was evident from the description of the data set. By writing out a grid of which numbers which letters appeared in, I found a sequence of deductions using the occurence pattern of the characters in the strings alone to arrive at the answer (and thereby avoid needing to track which numbers are associated with which strings as these are deduced). 

After determining the character decoding, the answer strings are then decoded and converted to a single base 10 number for aggregation at output. Some optimisation would be possible as there are searches into the existant mapping array when the required information had existed in the past and could be passed forward, but I think on balance the cost of the map search is minimal compared to the clutter of passing around the extra information.

</details>

<details>
	<summary>Day 9</summary>

Day 9 provided a fairly straightforward part 1, with part 2 presenting a greater challenege. Although it was fairly easy to find a way to solve part 2, the brute force approach seemed extremely inefficient. My planned approach was to find basins by picking a start point in each basin and then searching outwards from it, building up a list of points in that basin and then ending the search when no more points no already in the list could be found. In this way, a list of basins (where each basin is a list of its constituent points) could be forned. 

The key efficiency realisation was that since every basin must contain a low point, taking an output of the list of low points from part 1 could be used to drive the initial start points to the search algorithm as opposed to simply trying every point and doing the iteration if it wasn't already in a basin list.

</details>

<details>
	<summary>Day 10</summary>

Day 10 was very straight forward, though a bit more fiddly than other easy days such as 6 and 7. The only thing that caused an issue was in my initial solution for part 2, the score calculation was silently overflowing leading to an incorrect final scores being generated. After switching all of the relevant variables to 64bit this problem resolved.

A potential minor optimisation would be to find a way to neatly filter the array to the incomplete only strings ready for part 2 using the score array generated for part 1 - the easiest way to do this would be using a filteri style function, though there isn't one available in F# by default for some reason. The solution executes instantly in any case so not an important issue to fix.

</details>

<details>
	<summary>Day 10</summary>

Day 11 was not conceptually challenging, but proved tricky to find the optimal way to construct the loop structure required to evaluate the solution. I think this was a consequence of needing to pass all variables to be tracked over time around the iterator which resulted in more syntactic complexity than a comparative solution with mutable values persisting between loops. 

This was another day where attempting to use the built in 2D array in F# proved unweildly as the options for iterators on nested arrays are far more powerful. It seems increasingly to me that this construct is generally a red herring except in niche circumstances?

</details>