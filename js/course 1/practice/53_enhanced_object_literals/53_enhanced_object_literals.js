const photosGallery = (title, dimensions, date) => {
    return {
        title,
        date,
        [dimensions]: true,
        info() {
            console.log(
                `Photo "${title}" has resolution ${date}`
            );
        },
        publishInfo() {
            console.log(
                `Photo "${title}" was published ${Math.floor(
                    new Date().getTime() - date.getTime()) / 1000
                } seconds ago`
            )
        }
    }
}

const myDogPhoto = photosGallery('My dog', `1920x1080`, new Date())

const testDimension1 = `1920x1080`
const testDimension2 = '1080x720'

myDogPhoto.info()

setTimeout(() => myDogPhoto.publishInfo(), 2000)

console.log(myDogPhoto[testDimension1])
console.log(myDogPhoto[testDimension2])

console.log(Object.keys(myDogPhoto))