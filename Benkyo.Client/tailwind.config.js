/** @type {import('tailwindcss').Config} */
module.exports = {
    content: [
        "./Components/**/*.razor",
        "./Pages/**/*.razor",
        "./Shared/**/*.razor",
        "./Layout/**/*.razor",
        "./wwwroot/index.html"     
    ],
    theme: {
        extend: {},
    },
    plugins: [],
}