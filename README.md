# SortingStation

This is an implementation of Shunting-yard algorithm (or sorting stattion) that evaluate mathematicals expressions specified in infix notation. Written in C#.

For more read https://en.wikipedia.org/wiki/Shunting-yard_algorithm


Examples:

Input:
5+10*(1+1)-3*2

Output:
   5
   Add
   10
   Multiply
   (
   1
   Add
   1
   )
   Subtract
   3
   Multiply
   2

 value = 19
_______________________
Input:
 1,5*2

Output:
   1,5
   Multiply
   2

 value = 3
_______________________

Input:
5**5
Output:
   5
   Error
   Multiply
   Error
   Error
System.Exception: error in evaluation
Can't evaluate