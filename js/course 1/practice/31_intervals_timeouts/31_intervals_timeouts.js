
let i = 1
const messageIntervalId = setInterval(() => {
    console.log('Message ' + i)
    i++
    if (i === 5)
        return
}, 2000)

setTimeout(()=>clearInterval(messageIntervalId), 10000)