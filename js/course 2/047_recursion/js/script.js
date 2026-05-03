'use strict'

const pow = (x, n) => {
    let result = 1;

    for (let i = 0; i < n; i++) {
        result *= x
    }

    return result;
}

//console.log(pow(4, 2));
//console.log(pow(2, 3));
//console.log(pow(2, 4));
//console.log(pow(2, 0));

const powRecursion = (x, n) => {
    if (n <= 0)
        return 1
    if (n === 1) {
        return x;
    } else {
        return x * powRecursion(x, n - 1)
    }
}

//console.log(powRecursion(4, 2));
//console.log(powRecursion(2, 3));
//console.log(powRecursion(2, 4));
//console.log(powRecursion(2, 0));

let students = {
    js: [{
        name: 'John',
        progress: 100
    }, {
        name: 'Dan',
        progress: 60
    }],

    html: {
        basic: [{
            name: 'Peter',
            progress: 20
        }, {
            name: 'Ann',
            progress: 18
        }],
        pro: [{
            name: 'Sam',
            progress: 10
        }]
    }
};

function getTotalProgressByIteration(data) {
    let total = 0;
    let students = 0;

    const values = Object.values(data);
    for (let course of values) {
        if (Array.isArray(course)) {
            students += course.length;
            for (let i = 0; i < course.length; i++) {
                total += parseInt(course[i].progress)
            }
        } else {
            for (let subCourse of Object.values(course)) {
                students += subCourse.length;
                for (let i = 0; i < subCourse.length; i++) {
                    total += parseInt(subCourse[i].progress)
                }
            }
        }
    }

    return total / students;
}

//console.log(getTotalProgressByIteration(students));

function getTotalProgressByRecursion(data) {
    if (Array.isArray(data)) {
        let total = 0;
        for (let i = 0; i < data.length; i++) {
            total += parseInt(data[i].progress)
        }

        return [total, data.length]
    } else {
        let total = [0, 0]
        for (let subData of Object.values(data)) {
            const subDataArray = getTotalProgressByRecursion(subData)
            total[0] += subDataArray[0];
            total[1] += subDataArray[1];
        }
        return total;
    }
}

const result = getTotalProgressByRecursion(students);

console.log(result[0] / result[1]);

