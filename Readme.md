# Advent Of Code 2021

## Introduction

This project contains all of my solutions for the Advent of Code 2021 puzzle set (www.adventofcode.com) combined into a single project alongside my personalised input data. The entire project has been written in F# and is targeting a purely functional approach, with no use of mutable values or side effects (aside from console I/O with the User).

This code is all solely my own work, and you can reference or use it to whatever extent it may be of use for you to do so. 

## Structure

The executable entry point is in the MainProgram.fs file. This file contains the user input loop which allows the User to specify which days results are to be calculated and displayed. The calculations for each day are contained in separate source files, one for each day.

The input data is kept in the repository, with the pathing to this data handled by a routine in the MainProgram file. It is currently set up relative to the project folder so the project should build and run in Visual Studio fine, but a more robust solution may be implemented later.

## Notes on the Puzzles

(Obviously, spoilers for the puzzle solutions)

#### Day 1

A simple opening puzzle. The key observation which greatly simplifies the solution is to realise that if s_n(i) and s_n(i+1) are the sum of the n elements starting from index i and i+1 respectively, then s_n(i) < s_n(i+1) iff element(i) < element(i + n). This allows for variable length sums to be omitted entirely and the result array to build rapidly from single comparisons. 

#### Day 2

This puzzle had some finesse in getting the update step formulae correct and tracking the parameters, but otherwise unremarkable algorithmically. The input instructions, having been formatted into an array with one instruction per element, are then folded through with the sub state held in the accumulator as a tuple of the relevant values. The only difference between parts 1 and 2 is the update function in the accumulator performing different calculations.

#### Day 3

The first puzzle with some weight to it. Part 1 was fairly straight forward but part 2 required extra work around building the iterator. An optimisation here for part 2 would be to split out the process of evaluating the most common char in a given column from the function that calcualtes it for all columns as this since part 2 only uses them one at a time and they need recomputing each time anyway. However, given the execution speed is already unnoticeable with the full input set, this has not been done.

#### Day 4

This puzzle, while not as hard as day 3, did pose some challenges, particularly around wrangling the input data in the first instance. I eventually settled on using the built in 2D arrays to store the bingo cards as this made it easier to manage the boolean tick array later. The array slicing notation proved very useful for doing the win tests and makes this part of the code quite elegant.

The code would run faster if the boolean tick array was mutated each iteration rather than being rebuilt, but in the interests of pure functionaltiy I have left this as is.


#### Day 5
Day 5 is the first problem that takes a non-negligable amount of time to process. It also has been written so that it only outputs the part 2 solution and no longer produces the part 1 answer. The overhead arises from the fact that the rebuild of the array to update for each new line is being done over a c.1000x1000 so has a far greater impact than the 5x5 bingo cards in Day 4.

That inefficiency aside, the logic for identifying points on the lines was aided greatly by pre-processing the lines during data import to ensure they always ran left to right. Although the line test logic is explicitly split out into 4 separate cases, on reflection there is a lot of similarity so condensing it might be possible.

#### Day 6
My first theorised solution was to build an array with 1 entry per fish to track each fish's reproduction timer and extend the array each step. Before beginning on the problem, I then realised that the fish being homogeneous meant that it sufficed to track how many were at each point in the cycle and therefore track a fixed length array of the cohorts. This turned out to be a good move as the final number of fish became extremely large.

In part 2, the number became so large it became necessary to move all the calculations to 64 bit integers to prevent overflow, but otherwise the algorithm was identical.

#### Day 7
Day 7 was an easy one. The zero sum nature of the shift totals in the first part made it clear to see by an informal inductive argument that the median of the data set would always be the answer. The second part was not quite so receptive to analytic examination. It appears that answer is near the mean, but it's not exact. Given the brute force computation (accelerated by using the triangle number formula to get the fuel consumption) executes imperceptibly fast, I didn't bother to examine in more detail.

#### Test for collapsable section on github:
<details>
	<summary>Test Section</summary>

	Text that might be hidden?
</details>