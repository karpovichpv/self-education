$(document).ready(function () {

    console.log("hello everyone");

    const theForm = $("#theForm");
    theForm.hide();

    const button = $("#buyButton")
    button.on("click", function () {
        console.log('Buying item');
    });

    const productInfo = $(".product-props li");
    productInfo.on("click", function () {
        console.log("You clicked on" + $(this).text());
    });

    const $loginToggle = $("#loginToggle");
    const $popupForm = $(".popup-form");

    $loginToggle.on("click", function () {
        $popupForm.slideToggle(1000);
    })
});
