
module.exports = {
    entry: "./Content/src/food.tsx",
    mode: "development",
    output: {
        filename: "bundle.js"
    },
    resolve: {
        extensions: ['.ts', '.js', '.tsx', '.jsx']
    },

    devtool: 'source-map',

    // Add the loader for .ts files.
    module: {
        rules: [
            {
                test: /\.tsx?$/,
                loader: 'awesome-typescript-loader',

            },
            {
                test: /\.(sass|less|css)$/,
                loaders: ['style-loader', 'css-loader', 'less-loader']
            }
        ]
    }
};