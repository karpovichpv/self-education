const popularPostsIds = (posts, minPostsQuantities) => {
    return posts.reduce(
        (postsIds, post) =>
            post.comments >= minPostsQuantities
                ? postsIds.concat([post.postId])
                : postsIds,
        [])
}

const inputPosts = [
    {
        title: 'How fast learn JS?',
        postId: 3421,
        comments: 25,
    },
    {
        title: 'Where is used JS?',
        postId: 5216,
        comments: 3,
    },
    {
        title: 'What is difference between React and Angular?',
        postId: 8135,
        comments: 12,
    },
]

console.log(popularPostsIds(inputPosts, 10)) // [3421, 8135]
console.log(popularPostsIds(inputPosts, 15)) // [3421]
console.log(popularPostsIds(inputPosts, 50)) 