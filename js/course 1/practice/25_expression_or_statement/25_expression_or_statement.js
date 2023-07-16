23 // expression statement

// statement
const object = { // <-- expression
    x: 10,
    y: true,
}

// expression statement
myObject.z = 'abc' // <-- expression

// expression statement
delete myObject.x

// statement
let newVariable

// expression statement
newVariable = 30 + 5 //<-- expression

// expression statement
console.log(newVariable)
/**             ^
 *           expression
 *   
 */

// statement
if (newVariable > 10) {
    /**   ^
     *  expression 
     */

    // expression statement
    console.log(`${newVariable} more than 10`)
    /**         ----------------
     *              variable
     */
}