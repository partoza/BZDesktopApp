/** @type {import('tailwindcss').Config} */
module.exports = {
    content: [
        "./**/*.{razor,html,cshtml,js,ts}",
    ],
    theme: {
        extend: {
            colors: {
                primary: "#2f7d6d",
            },
            fontFamily: {
                poppins: ["Poppins", "ui-sans-serif", "system-ui", "-apple-system", "Segoe UI", "Roboto", "Helvetica Neue", "Arial", "Noto Sans", "Apple Color Emoji", "Segoe UI Emoji", "Segoe UI Symbol"],
            },
        },
    },
    plugins: [
        require('flowbite/plugin')
    ],
}
