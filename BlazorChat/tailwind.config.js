/** @type {import('tailwindcss').Config} */
module.exports = {
    content: ["./**/*.{razor,html,cshtml}"],
    theme: {
        extend: {
            animation: {
                bounce: 'bounce 0.6s infinite'
            },
            keyframes: {
                bounce: {
                    '0%, 100%': { transform: 'translateY(-25%)', opacity: '0.3' },
                    '50%': { transform: 'translateY(0)', opacity: '1' },
                }
            }
        }
    },
    plugins: [
        require("@tailwindcss/typography")
    ],
}

