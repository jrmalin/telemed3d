import numpy as np
import pandas as pd
import warnings


def is_int(x):
  try:
    int(x)
    return True
  except ValueError:
    return False


def parse_features(filename):
  # Read in features from PDF's CSV and remove malformed
  features = pd.read_csv(filename)
  features = features.dropna(subset=['ITEM'])
  features = features[features['ITEM'].apply(lambda x: is_int(x))]

  features = features.rename(columns={
    'ITEM': 'ITEM NO.',
    'FIELD': 'FIELD LENGTH',
    'FILE': 'FILE LOCATION',
    'Unnamed: 3': '[ITEM NAME], DESCRIPTION, AND CODES'
    })

  # Find data errors in features
  for i in range(1, 1067):
    count = len(features[features['ITEM NO.'] == str(i)])
    if count != 1:
      w = str(count) + ' items numbered "' + str(i) + '" instead of 1.'
      warnings.warn(w, Warning)

  if len(features.isnull().any(1).nonzero()[0]) > 0:
    w = 'nan values at printed indices'
    print(features.isnull().any(1).nonzero())
    warnings.warn(w, Warning)
    print(features.iloc[features.isnull().any(1).nonzero()[0][0]])

  return features


def main():
  features = parse_features('tabula_desc.csv')
  print(features.iloc[0:5])


if __name__ == "__main__":
  main()


# data = np.empty([1066, len(lines)])
# print('shape:', data.shape)

# f = open('namcs2015', 'r')
# for line in f.readlines():


