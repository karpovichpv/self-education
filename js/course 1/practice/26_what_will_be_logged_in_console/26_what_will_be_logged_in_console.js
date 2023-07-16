function fn(){
    console.log('Hello from function fn')

    return function (a) {
        console.log(a)
    }
}

fn()(true)