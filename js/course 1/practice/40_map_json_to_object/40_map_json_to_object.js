const postJSON =[
    '{"postId":1355,"commentsQuantity":5}',
    '{"postId":135555,"commentsQuantity":31}',
    '{"postId":135515,"commentsQuantity":31}',
    '{"postId":355,"commentsQuantity":31}'
]

const json = JSON.parse(postJSON)
console.log(json)