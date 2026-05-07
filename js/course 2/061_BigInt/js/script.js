'use strict';

console.log(Number.MAX_SAFE_INTEGER);
const bigint = 123333512356626124642612461234123412612463n;
const sameBigint = BigInt(123333512356626124642612461234123412612463);

console.log(typeof (bigint));
console.log(bigint + sameBigint);
console.log(Number(bigint) + 1);