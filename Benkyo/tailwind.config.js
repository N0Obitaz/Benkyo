/** @type {import('tailwindcss').Config} */
module.exports = {
    content: [
        // Server project paths
        "./Components/**/*.razor",
        "./Components/**/*.html",
        "./Pages/**/*.razor",
        "./Shared/**/*.razor",
        "./Layout/**/*.razor",
        // Client (WASM) project path - Adjust folder name if different
        "../Benkyo.Client/**/*.razor",
    ],
    darkMode: 'class', // or 'media' for automatic OS preference
    theme: {
        extend: {
            colors: {
                // Brand Colors
                primary: {
                    50: '#f0f9ff',
                    100: '#e0f2fe',
                    200: '#bae6fd',
                    300: '#7dd3fc',
                    400: '#38bdf8',
                    500: '#0ea5e9',  // Main brand color
                    600: '#0284c7',
                    700: '#0369a1',
                    800: '#075985',
                    900: '#0c4a6e',
                    950: '#082f49',
                },
                // Light Theme Colors
                light: {
                    background: '#ffffff',
                    surface: '#f9fafb',      // Cards, elevated elements
                    border: '#e5e7eb',
                    text: {
                        primary: '#111827',
                        secondary: '#6b7280',
                        tertiary: '#9ca3af',
                    },
                    hover: '#f3f4f6',
                },
                // Dark Theme Colors
                dark: {
                    background: '#0f172a',    // Main background
                    surface: '#1e293b',       // Cards, elevated elements
                    surfaceHover: '#334155',  // Hover states
                    border: '#334155',
                    text: {
                        primary: '#f8fafc',
                        secondary: '#cbd5e1',
                        tertiary: '#94a3b8',
                    },
                    hover: '#1e293b',
                },
                // Semantic Colors (work in both themes)
                success: {
                    light: '#10b981',
                    dark: '#34d399',
                },
                warning: {
                    light: '#f59e0b',
                    dark: '#fbbf24',
                },
                error: {
                    light: '#ef4444',
                    dark: '#f87171',
                },
                info: {
                    light: '#3b82f6',
                    dark: '#60a5fa',
                },
            },
        },
    },
    plugins: [],
}