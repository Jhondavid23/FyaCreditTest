/** @type {import('tailwindcss').Config} */
export default {
  content: [
    "./index.html",
    "./src/**/*.{js,ts,jsx,tsx}",
  ],
  theme: {
    extend: {
      colors: {
        'fya-blue': {
          800: '#1e40af',
          900: '#1e3a8a'
        }
      }
    },
  },
  plugins: [],
}