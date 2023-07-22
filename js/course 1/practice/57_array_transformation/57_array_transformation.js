
const processPosts = (posts) => {
    return posts.map((post) => {
        const {
            postId,
            author: postAuthor,
            commentsQty: postCommentsQty = 0
        } = post

        return {
            postAuthor,
            postCommentsQty,
            postId: postId + 1000,
        }
    })
}

const testPosts = [
    {
        postId: 234,
        author: 'robd',
        commentsQty: 5,
    },
    {
        postId: 823,
        author: 'sady'
    },
    {
        postId: 161,
        author: 'merryl',
        commentsQty: 8,
    }
]

const processedPosts = processPosts(testPosts)
console.log(processedPosts)