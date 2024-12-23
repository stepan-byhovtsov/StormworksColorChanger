﻿# How to use

1. Create color mapping file with *.json* extension.
2. If there is a "colors.json" file, it will be used by default. Otherwise, the program will ask you to specify color mappings file (relative path with extension, like *colors1*)
3. If no arguments were passed to program, it will ask you to specify vehicle save name without file extension (like *bmp_v1_aps*, it will try to open "%appdata%/Stormworks/data/vehicles/bmp_v1_aps.xml")
4. If everything was ok, the program will recolor vehicle and save it to *vehiclename*_rc save

## Color mapping files
### Sample mapping file:
```json
{
  "0,94,115": {
    "$randSat": {
      "base": "0,94,115",
      "deviation" : 50
    }
  },
  "FFFFFF": {
    "$randSat": {
      "base": "f1dfd4",
      "deviation" : 50
    }
  },
  "515C4c": {
    "$randSat": {
      "base": "515C4C",
      "deviation": 50
    }
  },
  "255,0,0": "0,255,0",
  "0,255,0": "FFFFFF"
}
```
### Syntax:
JSON object. Keys are colors that will be replaced.
Colors can be provided in following syntaxes:
- RRGGBB (hex, e.g. FF0091)
- R,G,B (decimal from 0 to 255, e.g. 127, 3, 15)

Values can be either constant color or a special macros.
#### Constant color syntax:
`"RRGGBB":"rrggbb"` * Note: You can also use second color syntax: `"RRGGBB":"r,g,b"`

#### Random saturation operator
```json
  "[color to replace]" : {
    "$randSat": {
      "base": "[base color]",
      "deviation": [saturation deviation]
    }
  }
```

#### Random from set operator
```json
"255,255,255": {
    "$randFrom": [
      {
        "//": "Probability will be calculated automatically",
        "$randSat": {
          "base": "227, 224, 232", "//": "SnowColor",
          "deviation": 50
        }
      },
      {
        "probability": 0.25, "//": "Amount of color",
        "$randSat": {
          "base": "49, 63, 11", "//": "DarkPineColor",
          "deviation": 50
        }
      }
    ]
  }
```

For each replaced color occurence, selects and generates a random color from a provided set.
