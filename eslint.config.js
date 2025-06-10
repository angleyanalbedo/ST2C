/** @type {import("eslint").Linter.FlatConfig[]} */
export default [
  {
    files: ["src/*.ts"],
    languageOptions: {
      ecmaVersion: 2021,
      sourceType: "module",
      globals: {
        window: "readonly",
        document: "readonly",
        console: "readonly",
        module: "writable",
        require: "readonly",
        process: "readonly"
      }
    },
    plugins: {},
    rules: {
      // "semi": ["error", "always"],
      // "quotes": ["error", "single"]
    }
  }
];