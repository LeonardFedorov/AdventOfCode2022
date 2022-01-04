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

Day 5 is the first problem that takes a non-negligable amount of time to process. The overhead arises from the fact that the rebuild of the array to update for each new line is being done over a c.1000x1000 so has a far greater impact than the 5x5 bingo cards in Day 4.

That inefficiency aside, the logic for identifying points on the lines was aided greatly by pre-processing the lines during data import to ensure they always ran left to right. Although the line test logic is explicitly split out into 4 separate cases, on reflection there is a lot of similarity so condensing it might be possible.

The initial solution to this part replaced the part 1 solution with the part 2, but I have tweaked it so that it can generate both, albeit in an inefficient manner. It would be more efficient to generate the part 1 map, then overlay the diagonal consideration onto this, but this would require much of a rework of the logic.

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
	<summary>Day 11</summary>

Day 11 was not conceptually challenging, but proved tricky to find the optimal way to construct the loop structure required to evaluate the solution. I think this was a consequence of needing to pass all variables to be tracked over time around the iterator which resulted in more syntactic complexity than a comparative solution with mutable values persisting between loops. 

This was another day where attempting to use the built in 2D array in F# proved unweildly as the options for iterators on nested arrays are far more powerful. It seems increasingly to me that this construct is generally a red herring except in niche circumstances?

</details>

<details>
	<summary>Day 12</summary>

Day 12 wasn't hard conceptually, as it could be solved with a fairly routine branching recursion, however I found some challenge in thinking of a way to express this somewhat elegently in the language. I settled on splitting the recursion across two functions - one of which processed the items and put forward a list of items for further processing, and a second function that took the list and dispatched instances of the first function to follow up on them. 

Part 2, while irritating at first glance, actually was fairly simple to implement as it only required changing the filtering logic when selecting which small caves to put forward for further consideration, although there some fiddliness getting the logic to work exactly. The two parts run using the same code, with a parameter governing how many times the multiplied small cave is allowed to be visited.

</details>

<details>
	<summary>Day 13</summary>

I found the explanation for Day 13 made it unclear where exactly the fold line was and how this aligned with a 0 based array (I don't know if I missed some tiny detail in the explanation, but to me this wasn't clear). So I initially included a indexing adjustment that was unnecessary.

For part 2, my initial solution did not appear to work and it took about an hour to realse I had outputted the yes/no characters the wrong way round. Such is life sometimes...

</details>

<details>
	<summary>Day 14</summary>

This solution, of all to day, is the most readily accusable of being over-engineered. After initially building the string explicitly to solve part 1 - and then rapidly hitting the exponential growth brick wall when trying part 2 - I ended up with this monstrosity. In effect, it creates a giant mapping array in memory as part of the inital text parse which then allows the actual iteration steps to execute in linear time. The solution assumes that every possible pair mapping is specified in the input (which I verified by inspecting the input text in Excel) and that every letter is the result of at least one pair map. 

While an ugly solution, it does at least execute instantly - and would likely continue to execute instantly even on extremely large iterations as the iteration step simply performs addition of integers using lookups fixed at the start.

</details>

<details>
	<summary>Day 15</summary>

This problem ended up being a massive headache, but this was largely self inflicted from attacking the problem piecemeal over a long period of time as other things came up. Given the performance implications of the problem, I ended up using single mutable data structure to track the progress of the algorithm over time, which turned out to be sensible as part 2 took several seconds to execute even with this. Further optimisations that could be made would be: 
1. Maintaining the active point list in a sorted state so that new points could be added by insertion and then the minimum point would simply be the head of the list
2. Actively pruning duplicate entries for a single point, which arise when a point is revisted, as these inflate the list and add headroom to the list operations. 

</details>

<details>
	<summary>Day 16</summary>

This day seemed quite fiddly on first inspection, but proved susceptible to a reasonably elegant solution in the end. A curve ball was that the evaluation step in part 2 required 64 bit airthmetic to prevent overflow, which I addressed by setting the function that parsed binary strings into numbers to output 64 bit integers. This had the somewhat fiddly side effect of requiring every integer literal in the code to be marked as long and is probably not fully memory optimal given the majority of the integers are extremely small. Given the solution executes instantly though, this is fairly academic. 

</details>

<details>
	<summary>Day 18</summary>

This problem proved very fiddly. This problem led me to discover discriminated unions, which proved vastly superior to using a recursive record with options within. The hardest part of the problem by far was dealing with the explosion operation, as it was necessary not only recursively search the tree for a pair eligable to be exploded, but then to carry the debris values back up the tree in order to find somewhere to place it. This results in a 4-tuple of data being passed back up the recursion stack and the need to process this. 

Part 2 was very straight forward at least, and was a solvable using a quick map construction.

</details>