const postJSON =[
    '{"postId":1355,"commentsQuantity":5}',
    '{"postId":135555,"commentsQuantity":31}',
    '{"postId":135515,"commentsQuantity":31}',
    '{"postId":355,"commentsQuantity":31}'
]

const json = postJSON.map(JSON.parse) 

console.log(json.find((element)=>element.postId === 1355))
